using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Commands
{
    public class MoveCommand : ICommand
    {
        public void Execute(Vector3 point, Character chara)
        {
            chara.NavigatePosition(point);
        }
    }
}