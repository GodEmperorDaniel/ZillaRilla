using UnityEngine;

namespace Entities.Scripts
{
	public interface IMoveInput
	{ 
		Vector2 MoveDirection { get; }
	}
}