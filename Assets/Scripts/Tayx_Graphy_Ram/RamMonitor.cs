using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Tayx.Graphy.Ram
{
	public class RamMonitor : MonoBehaviour
	{
		private float m_allocatedRam;

		private float m_reservedRam;

		private float m_monoRam;

		public float AllocatedRam
		{
			get
			{
				return this.m_allocatedRam;
			}
		}

		public float ReservedRam
		{
			get
			{
				return this.m_reservedRam;
			}
		}

		public float MonoRam
		{
			get
			{
				return this.m_monoRam;
			}
		}

		private void Update()
		{
			this.m_allocatedRam = (float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
			this.m_reservedRam = (float)Profiler.GetTotalReservedMemoryLong() / 1048576f;
			this.m_monoRam = (float)Profiler.GetMonoUsedSizeLong() / 1048576f;
		}
	}
}
