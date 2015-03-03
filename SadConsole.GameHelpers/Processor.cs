﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadConsole.GameHelpers
{
    class Processor :GameObject, IProcessor
    {
        public string Id { get; private set; }
        public IEnumerable<string> TargetIds { get; private set; }

        public IEnumerable<GameObject> ResolvedTargets { get; private set; }

        public Func<Trigger, GameObjectCollection, Consoles.Console, IEnumerable<GameObjectCollection>, bool> Condition;
        public Action<Trigger, GameObjectCollection, Consoles.Console, IEnumerable<GameObjectCollection>> Result;

        public Processor(GameObject source)
        {
            string id = "";
            StringBuilder targets = new StringBuilder();

            foreach (var setting in source.Settings)
            {
                string name = setting.Name.ToLower().Trim();
                if (name == "id")
                    id = setting.Value;
                else if (name == "target")
                    targets.Append(String.Format("{0};", setting.Value));
            }

            Id = id;
            TargetIds = targets.ToString().Trim(';').Trim().Split(';');

            source.CopyTo(this);
        }

        public void Triggered(ITrigger trigger, GameObjectCollection parent, Consoles.Console console, IEnumerable<GameObjectCollection> otherCollections = null)
        {

        }

        public override void Process(GameObjectCollection parent, Consoles.Console console, IEnumerable<GameObjectCollection> otherCollections = null)
        {
            //if (Condition(this, parent, console, otherCollections))
            //    Result()
        }
    }
}
