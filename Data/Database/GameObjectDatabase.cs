using UnityEngine;

//Готовый к использованию пример ScriptableDatabase

[CreateAssetMenu(fileName = "GameObjectDatabase", menuName = "Assets/GameObjectDatabase")]
public class GameObjectDatabase : ScriptableDatabase<GameObject>
{
}