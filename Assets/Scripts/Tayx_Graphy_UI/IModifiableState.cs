using System;

namespace Tayx.Graphy.UI
{
	public interface IModifiableState
	{
		void SetState(GraphyManager.ModuleState newState);
	}
}
