using UnityEngine;
using DamageMechanics;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyDeath : MonoBehaviour
{
    private void Awake()
    {
        var damageable = GetComponent<Damageable>();
        damageable.OnDeath.AddListener(OnEnemyDeath);
    }
        
    public void OnEnemyDeath(Damage demage)
    { 
        Destroy(this.gameObject);
        if (this.gameObject.name == "Boss1")
        {
            StartCoroutine(waiter());
            //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level 2");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");

            foreach (GameObject player in players)
            {
                Destroy(player);
            }

            foreach (GameObject entity in entities)
            {
                Destroy(entity);
            }
            
            GameScript.Instance.ShowVictoryMenu();
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(5f);
    }
}
