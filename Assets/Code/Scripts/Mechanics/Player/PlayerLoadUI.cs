using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMechanics
{
    public class PlayerLoadUI : MonoBehaviour
    {
        public GameObject playerHealthBarPrefab;

        // Start is called before the first frame update
        void Start()
        {
            GameObject playerHealthBar = Instantiate(playerHealthBarPrefab, transform);
        }

    }
}