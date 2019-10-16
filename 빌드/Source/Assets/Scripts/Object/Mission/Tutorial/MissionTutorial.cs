using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Mission
{
    public enum TutorialState
    {
        None = 0,
        FirstTutorial,
    }

    public class MissionTutorial : MissionBase
    {
        public TutorialState currentTutorial = TutorialState.None;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            currentTutorial = TutorialState.None;
        }

        // Update is called once per frame
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}