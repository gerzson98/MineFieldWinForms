﻿namespace MineField.Model.Entities
{
    public class Player
    {
        public string Name { get; private set; }
        public Player(string name)
        {
            Name = name;
        }
    }
}
