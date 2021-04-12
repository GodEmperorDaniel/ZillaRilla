using UnityEngine;

namespace Entities.Scripts
{
	public interface IMoveInput
	{ 
		Vector3 MoveDirection { get; }
	}
}