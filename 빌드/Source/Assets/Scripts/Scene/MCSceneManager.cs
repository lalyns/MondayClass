using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MC.SceneDirector
{
    public class MCSceneManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StartSceneButton()
        {
            SceneManager.LoadScene(1);
        }
    }
}