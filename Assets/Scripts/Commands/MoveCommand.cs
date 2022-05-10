using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Commands
{
    public class MoveCommand : ICommand
    {
        // private readonly Vector3[] points;
        // public MoveCommand(Character[] selected)
        // {
        //     selectedCharas = selected;
        // }
        public void Execute(Vector3 point, Character chara)
        {
            chara.NavigatePosition(point);
        }
    }
}