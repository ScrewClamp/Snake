using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Tayx.Graphy.Audio;
using Tayx.Graphy.Fps;
using Tayx.Graphy.Ram;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Tayx.Graphy
{
	public class GraphyDebugger : Singleton<GraphyDebugger>
	{
		public enum DebugVariable
		{
			Fps,
			Fps_Min,
			Fps_Max,
			Fps_Avg,
			Ram_Allocated,
			Ram_Reserved,
			Ram_Mono,
			Audio_DB
		}

		public enum DebugComparer
		{
			Less_than,
			Equals_or_less_than,
			Equals,
			Equals_or_greater_than,
			Greater_than
		}

		public enum ConditionEvaluation
		{
			All_conditions_must_be_met,
			Only_one_condition_has_to_be_met
		}

		public enum MessageType
		{
			Log,
			Warning,
			Error
		}

		[Serializable]
		public struct DebugCondition
		{
			[Tooltip("Variable to compare against")]
			public GraphyDebugger.DebugVariable Variable;

			[Tooltip("Comparer operator to use")]
			public GraphyDebugger.DebugComparer Comparer;

			[Tooltip("Value to compare against the chosen variable")]
			public float Value;
		}

		[Serializable]
		public class DebugPacket
		{
			[Tooltip("If false, it won't be checked")]
			public bool Active = true;

			[Tooltip("Optional Id. It's used to get or remove DebugPackets in runtime")]
			public int Id;

			[Tooltip("If true, once the actions are executed, this DebugPacket will delete itself")]
			public bool ExecuteOnce = true;

			[Tooltip("Time to wait before checking if conditions are met (use this to avoid low fps drops triggering the conditions when loading the game)")]
			public float InitSleepTime = 2f;

			[Tooltip("Time to wait before checking if conditions are met again (once they have already been met and if ExecuteOnce is false)")]
			public float ExecuteSleepTime = 2f;

			public GraphyDebugger.ConditionEvaluation ConditionEvaluation;

			[Tooltip("List of conditions that will be checked each frame")]
			public List<GraphyDebugger.DebugCondition> DebugConditions = new List<GraphyDebugger.DebugCondition>();

			public GraphyDebugger.MessageType MessageType;

			[Multiline]
			public string Message = string.Empty;

			public bool TakeScreenshot;

			public string ScreenshotFileName = "Graphy_Screenshot";

			[Tooltip("If true, it pauses the editor")]
			public bool DebugBreak;

			public UnityEvent UnityEvents;

			public List<Action> Callbacks = new List<Action>();

			private bool canBeChecked;

			private bool executed;

			private float timePassed;

			public bool Check
			{
				get
				{
					return this.canBeChecked;
				}
			}

			public void Update()
			{
				if (!this.canBeChecked)
				{
					this.timePassed += Time.deltaTime;
					if ((this.executed && this.timePassed >= this.ExecuteSleepTime) || (!this.executed && this.timePassed >= this.InitSleepTime))
					{
						this.canBeChecked = true;
						this.timePassed = 0f;
					}
				}
			}

			public void Executed()
			{
				this.canBeChecked = false;
				this.executed = true;
			}
		}

		private sealed class _GetFirstDebugPacketWithId_c__AnonStorey0
		{
			internal int packetId;

			internal bool __m__0(GraphyDebugger.DebugPacket x)
			{
				return x.Id == this.packetId;
			}
		}

		private sealed class _GetAllDebugPacketsWithId_c__AnonStorey1
		{
			internal int packetId;

			internal bool __m__0(GraphyDebugger.DebugPacket x)
			{
				return x.Id == this.packetId;
			}
		}

		private sealed class _RemoveAllDebugPacketsWithId_c__AnonStorey2
		{
			internal int packetId;

			internal bool __m__0(GraphyDebugger.DebugPacket x)
			{
				return x.Id == this.packetId;
			}
		}

		private FpsMonitor m_fpsMonitor;

		private RamMonitor m_ramMonitor;

		private AudioMonitor m_audioMonitor;

		[SerializeField]
		private List<GraphyDebugger.DebugPacket> m_debugPackets;

		protected GraphyDebugger()
		{
		}

		private void Start()
		{
			this.m_fpsMonitor = base.GetComponentInChildren<FpsMonitor>();
			this.m_ramMonitor = base.GetComponentInChildren<RamMonitor>();
			this.m_audioMonitor = base.GetComponentInChildren<AudioMonitor>();
		}

		private void Update()
		{
			this.CheckDebugPackets();
		}

		public void AddNewDebugPacket(GraphyDebugger.DebugPacket newDebugPacket)
		{
			this.m_debugPackets.Add(newDebugPacket);
		}

		public void AddNewDebugPacket(int newId, GraphyDebugger.DebugCondition newDebugCondition, GraphyDebugger.MessageType newMessageType, string newMessage, bool newDebugBreak, Action newCallback)
		{
			this.AddNewDebugPacket(new GraphyDebugger.DebugPacket
			{
				Id = newId,
				DebugConditions = 
				{
					newDebugCondition
				},
				MessageType = newMessageType,
				Message = newMessage,
				DebugBreak = newDebugBreak,
				Callbacks = 
				{
					newCallback
				}
			});
		}

		public void AddNewDebugPacket(int newId, List<GraphyDebugger.DebugCondition> newDebugConditions, GraphyDebugger.MessageType newMessageType, string newMessage, bool newDebugBreak, Action newCallback)
		{
			this.AddNewDebugPacket(new GraphyDebugger.DebugPacket
			{
				Id = newId,
				DebugConditions = newDebugConditions,
				MessageType = newMessageType,
				Message = newMessage,
				DebugBreak = newDebugBreak,
				Callbacks = 
				{
					newCallback
				}
			});
		}

		public void AddNewDebugPacket(int newId, GraphyDebugger.DebugCondition newDebugCondition, GraphyDebugger.MessageType newMessageType, string newMessage, bool newDebugBreak, List<Action> newCallbacks)
		{
			this.AddNewDebugPacket(new GraphyDebugger.DebugPacket
			{
				Id = newId,
				DebugConditions = 
				{
					newDebugCondition
				},
				MessageType = newMessageType,
				Message = newMessage,
				DebugBreak = newDebugBreak,
				Callbacks = newCallbacks
			});
		}

		public void AddNewDebugPacket(int newId, List<GraphyDebugger.DebugCondition> newDebugConditions, GraphyDebugger.MessageType newMessageType, string newMessage, bool newDebugBreak, List<Action> newCallbacks)
		{
			this.AddNewDebugPacket(new GraphyDebugger.DebugPacket
			{
				Id = newId,
				DebugConditions = newDebugConditions,
				MessageType = newMessageType,
				Message = newMessage,
				DebugBreak = newDebugBreak,
				Callbacks = newCallbacks
			});
		}

		public GraphyDebugger.DebugPacket GetFirstDebugPacketWithId(int packetId)
		{
			return this.m_debugPackets.First((GraphyDebugger.DebugPacket x) => x.Id == packetId);
		}

		public List<GraphyDebugger.DebugPacket> GetAllDebugPacketsWithId(int packetId)
		{
			return this.m_debugPackets.FindAll((GraphyDebugger.DebugPacket x) => x.Id == packetId);
		}

		public void RemoveFirstDebugPacketWithId(int packetId)
		{
			this.m_debugPackets.Remove(this.GetFirstDebugPacketWithId(packetId));
		}

		public void RemoveAllDebugPacketsWithId(int packetId)
		{
			this.m_debugPackets.RemoveAll((GraphyDebugger.DebugPacket x) => x.Id == packetId);
		}

		public void AddCallbackToFirstDebugPacketWithId(Action callback, int id)
		{
			this.GetFirstDebugPacketWithId(id).Callbacks.Add(callback);
		}

		public void AddCallbackToAllDebugPacketWithId(Action callback, int id)
		{
			foreach (GraphyDebugger.DebugPacket current in this.GetAllDebugPacketsWithId(id))
			{
				current.Callbacks.Add(callback);
			}
		}

		private void CheckDebugPackets()
		{
			List<GraphyDebugger.DebugPacket> list = new List<GraphyDebugger.DebugPacket>();
			foreach (GraphyDebugger.DebugPacket current in this.m_debugPackets)
			{
				if (current.Active)
				{
					current.Update();
					if (current.Check)
					{
						GraphyDebugger.ConditionEvaluation conditionEvaluation = current.ConditionEvaluation;
						if (conditionEvaluation != GraphyDebugger.ConditionEvaluation.All_conditions_must_be_met)
						{
							if (conditionEvaluation == GraphyDebugger.ConditionEvaluation.Only_one_condition_has_to_be_met)
							{
								foreach (GraphyDebugger.DebugCondition current2 in current.DebugConditions)
								{
									if (this.CheckIfConditionIsMet(current2))
									{
										this.ExecuteOperationsInDebugPacket(current);
										if (current.ExecuteOnce)
										{
											list.Add(current);
										}
										break;
									}
								}
							}
						}
						else
						{
							int num = 0;
							foreach (GraphyDebugger.DebugCondition current3 in current.DebugConditions)
							{
								if (this.CheckIfConditionIsMet(current3))
								{
									num++;
								}
							}
							if (num >= current.DebugConditions.Count)
							{
								this.ExecuteOperationsInDebugPacket(current);
								if (current.ExecuteOnce)
								{
									list.Add(current);
								}
							}
						}
					}
				}
			}
			foreach (GraphyDebugger.DebugPacket current4 in list)
			{
				this.m_debugPackets.Remove(current4);
			}
		}

		private bool CheckIfConditionIsMet(GraphyDebugger.DebugCondition debugCondition)
		{
			switch (debugCondition.Comparer)
			{
			case GraphyDebugger.DebugComparer.Less_than:
				return this.GetRequestedValueFromDebugVariable(debugCondition.Variable) < debugCondition.Value;
			case GraphyDebugger.DebugComparer.Equals_or_less_than:
				return this.GetRequestedValueFromDebugVariable(debugCondition.Variable) <= debugCondition.Value;
			case GraphyDebugger.DebugComparer.Equals:
				return Mathf.Approximately(this.GetRequestedValueFromDebugVariable(debugCondition.Variable), debugCondition.Value);
			case GraphyDebugger.DebugComparer.Equals_or_greater_than:
				return this.GetRequestedValueFromDebugVariable(debugCondition.Variable) >= debugCondition.Value;
			case GraphyDebugger.DebugComparer.Greater_than:
				return this.GetRequestedValueFromDebugVariable(debugCondition.Variable) > debugCondition.Value;
			default:
				return false;
			}
		}

		private float GetRequestedValueFromDebugVariable(GraphyDebugger.DebugVariable debugVariable)
		{
			switch (debugVariable)
			{
			case GraphyDebugger.DebugVariable.Fps:
				return this.m_fpsMonitor.CurrentFPS;
			case GraphyDebugger.DebugVariable.Fps_Min:
				return this.m_fpsMonitor.MinFPS;
			case GraphyDebugger.DebugVariable.Fps_Max:
				return this.m_fpsMonitor.MaxFPS;
			case GraphyDebugger.DebugVariable.Fps_Avg:
				return this.m_fpsMonitor.AverageFPS;
			case GraphyDebugger.DebugVariable.Ram_Allocated:
				return this.m_ramMonitor.AllocatedRam;
			case GraphyDebugger.DebugVariable.Ram_Reserved:
				return this.m_ramMonitor.AllocatedRam;
			case GraphyDebugger.DebugVariable.Ram_Mono:
				return this.m_ramMonitor.AllocatedRam;
			case GraphyDebugger.DebugVariable.Audio_DB:
				return this.m_audioMonitor.MaxDB;
			default:
				return 0f;
			}
		}

		private void ExecuteOperationsInDebugPacket(GraphyDebugger.DebugPacket debugPacket)
		{
			if (debugPacket.DebugBreak)
			{
				UnityEngine.Debug.Break();
			}
			if (debugPacket.Message != string.Empty)
			{
				string message = string.Concat(new object[]
				{
					"[Graphy] (",
					DateTime.Now,
					"): ",
					debugPacket.Message
				});
				GraphyDebugger.MessageType messageType = debugPacket.MessageType;
				if (messageType != GraphyDebugger.MessageType.Log)
				{
					if (messageType != GraphyDebugger.MessageType.Warning)
					{
						if (messageType == GraphyDebugger.MessageType.Error)
						{
							UnityEngine.Debug.LogError(message);
						}
					}
					else
					{
						UnityEngine.Debug.LogWarning(message);
					}
				}
				else
				{
					UnityEngine.Debug.Log(message);
				}
			}
			if (debugPacket.TakeScreenshot)
			{
				string text = string.Concat(new object[]
				{
					debugPacket.ScreenshotFileName,
					"_",
					DateTime.Now,
					".png"
				});
				text = text.Replace("/", "-").Replace(" ", "_").Replace(":", "-");
				ScreenCapture.CaptureScreenshot(text);
			}
			debugPacket.UnityEvents.Invoke();
			foreach (Action current in debugPacket.Callbacks)
			{
				if (current != null)
				{
					current();
				}
			}
			debugPacket.Executed();
		}
	}
}
