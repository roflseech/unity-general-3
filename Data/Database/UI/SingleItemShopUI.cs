using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public abstract class SingleItemShopUI<T> : MonoBehaviour where T : class
{
    [SerializeField]
    private Transform _itemsPanel;
    [SerializeField]
    private float _startPrice = 200.0f;
    [SerializeField]
    private float _priceCountMultiplier = 400.0f;
    [SerializeField]
    private CustomizableButton _itemButtonPrefab;
    [SerializeField]
    private Button _buyRandomButton;
    [SerializeField]
    private TMP_Text _priceText;

    private Dictionary<string, CustomizableButton> _itemsButtons = new Dictionary<string, CustomizableButton>();

    protected abstract IReadOnlyList<ScriptableDatabase<T>.DatabaseEntry> GetShopItems();
    protected abstract void SubscribeToSelectionUpdate(Action<string> action);
    protected abstract void UnsubscribeFromelectionUpdate(Action<string> action);
    protected abstract bool IsUnlocked(string name);
    protected abstract void SelectItem(string name);
    protected abstract string GetSelectedItem();
    protected abstract void UnlockItem(string name);
    protected abstract void OnItemBought();
    protected abstract void OnLockedItemClicked();
    protected abstract void OnNotEnoughMoney();
    protected abstract float GetCurrentMoney();
    protected abstract void SetCurrentMoney(float value);

    public void Start()
    {
        _buyRandomButton.onClick.AddListener(() => BuyRandom());
        foreach (var entry in GetShopItems())
        {
            var button = CreateButton(entry);
            button.transform.SetParent(_itemsPanel, false);
            _itemsButtons.Add(entry.Name, button);
        }

        UpdateBuyButton();
        UpdateCharacterButtons();
        SubscribeToSelectionUpdate(SelectedItemChanged);
    }
    private void OnDestroy()
    {
        UnsubscribeFromelectionUpdate(SelectedItemChanged);
    }
    private void SelectedItemChanged(string name)
    {
        UpdateCharacterButtons();
    }
    private CustomizableButton CreateButton(ScriptableDatabase<T>.DatabaseEntry entry)
    {
        var button = Instantiate(_itemButtonPrefab);
        button.SetText(entry.Name);
        button.SetIcon(entry.Icon);
        button.AddClickEvent(() => CharacterButtonClick(entry.Name));
        return button;
    }
    private void CharacterButtonClick(string name)
    {
        if (IsUnlocked(name))
        {
            SelectItem(name);
        }
        else
        {
            OnLockedItemClicked();
        }
    }
    private void UpdateCharacterButtons()
    {
        foreach (var button in _itemsButtons)
        {
            if (button.Key == GetSelectedItem())
            {
                button.Value.SetHighlight(true);
            }
            else
            {
                button.Value.SetHighlight(false);
            }

            if (IsUnlocked(button.Key))
            {
                button.Value.SetFade(false);
            }
            else
            {
                button.Value.SetFade(true);
            }
        }
    }

    private float GetNextPrice()
    {
        int count = 0;
        foreach (var entry in GetShopItems())
        {
            if (IsUnlocked(entry.Name)) count++;
        }
        return Mathf.Floor(_startPrice + _priceCountMultiplier * (count - 1));
    }
    private void BuyRandom()
    {
        List<int> options = new List<int>();
        int current = 0;
        foreach (var entry in GetShopItems())
        {
            if (!IsUnlocked(entry.Name)) options.Add(current);
            current++;
        }

        if (options.Count > 0)
        {
            float price = GetNextPrice();
            if (GetCurrentMoney() >= price)
            {
                int index = options[Random.Range(0, options.Count)];
                var item = GetShopItems()[index];
                UnlockItem(item.Name);
                SelectItem(item.Name);
                SetCurrentMoney(GetCurrentMoney() - price);
                UpdateBuyButton();
                OnItemBought();
            }
            else
            {
                OnNotEnoughMoney();
            }
        }
    }
    private void UpdateBuyButton()
    {
        int count = 0;
        foreach (var entry in GetShopItems())
        {
            if (IsUnlocked(entry.Name)) count++;
        }
        if (count < GetShopItems().Count)
        {
            _priceText.text = "Buy random(" + GetNextPrice().ToString() + ")";
        }
        else
        {
            _priceText.text = "All unlocked";
            _buyRandomButton.interactable = false;
            _buyRandomButton.image.color = Color.grey;
        }
    }
}