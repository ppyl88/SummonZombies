using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint; // 아군 AI를 소환할 위치들

    private List<Player> players = new List<Player>(); // 생성된 아군들을 담는 리스트

    // 플레이어를 생성
    public void CreatePlayer(int id)
    {
        Player player = Instantiate(GameManager.instance.humen[id].playerPrefab, spawnPoint.position, spawnPoint.rotation);

        player.Setup(GameManager.instance.humen[id].health, GameManager.instance.humen[id].damage, GameManager.instance.humen[id].speed, GameManager.instance.humen[id].timeBetAttack);

        players.Add(player);

        GameManager.instance.RemoveCoin(GameManager.instance.humen[id].cost);

        player.onDeath += () => players.Remove(player);
        player.onDeath += () => Destroy(player.gameObject, 3f);
    }
}
