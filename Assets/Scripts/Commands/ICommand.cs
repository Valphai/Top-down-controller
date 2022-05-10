using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Commands
{
    public interface ICommand 
    {
        public void Execute(Vector3 point, Character chara);
    }
}