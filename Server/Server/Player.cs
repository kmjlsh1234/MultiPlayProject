using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player
    {
        public Vector3 pos;
        public Quaternion rot;
        public float sprintSpeed = 5.335f;
        public float moveSpeed = 2.0f;

        public float currentMoveX;
        public float currentMoveY;
        public bool currentSprint;

        public Player(Vector3 spawnPos)
        {
            pos = spawnPos;
            rot = Quaternion.Identity;
        }

    }
}
