﻿/* 
 * This file was automatically generated by Jesus!
 * Do not modify, or your sins will be regenerated!!
 * XML: "..\..\Octgn.Library\Scripting\GameEvents.xml"
 *
 * To create the .CS file for this document, 
 * right click the .tt file and click 'Run Custom Tool'
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Octgn.Play;
using log4net;

namespace Octgn.Scripting
{
	public class GameEventProxy
	{
		internal static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly Engine engine;

		public bool MuteEvents {get;set;}
		public GameEventProxy(Engine scriptEngine)
		{
			engine = scriptEngine;
		}
	
		public void OnTableLoad()
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnTableLoad");
			    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnTableLoad"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnTableLoad"])
					{
						Log.InfoFormat("Firing event OnTableLoad -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction);
					}			}
		}

		public void OnGameStart()
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnGameStart");
			    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnGameStart"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnGameStart"])
					{
						Log.InfoFormat("Firing event OnGameStart -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction);
					}			}
		}

		public void OnLoadDeck(Player player, Group[] groups)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnLoadDeck");
			var args = new object[2];
			args[0] = player;
			args[1] = groups;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnLoadDeck"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnLoadDeck"])
					{
						Log.InfoFormat("Firing event OnLoadDeck -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, groups);
					}			}
		}

		public void OnChangeCounter(Player player, Counter counter, int oldValue)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnChangeCounter");
			var args = new object[3];
			args[0] = player;
			args[1] = counter;
			args[2] = oldValue;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnChangeCounter"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnChangeCounter"])
					{
						Log.InfoFormat("Firing event OnChangeCounter -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, counter, oldValue);
					}			}
		}

		public void OnEndTurn(Player player)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnEndTurn");
			var args = new object[1];
			args[0] = player;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnEndTurn"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnEndTurn"])
					{
						Log.InfoFormat("Firing event OnEndTurn -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player);
					}			}
		}

		public void OnTurn(Player player, int turnNumber)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnTurn");
			var args = new object[2];
			args[0] = player;
			args[1] = turnNumber;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnTurn"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnTurn"])
					{
						Log.InfoFormat("Firing event OnTurn -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, turnNumber);
					}			}
		}

		public void OnTargetCard(Player player, Card card, bool isTargeted)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnTargetCard");
			var args = new object[3];
			args[0] = player;
			args[1] = card;
			args[2] = isTargeted;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnTargetCard"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnTargetCard"])
					{
						Log.InfoFormat("Firing event OnTargetCard -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, card, isTargeted);
					}			}
		}

		public void OnTargetCardArrow(Player player, Card fromCard, Card toCard, bool isTargeted)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnTargetCardArrow");
			var args = new object[4];
			args[0] = player;
			args[1] = fromCard;
			args[2] = toCard;
			args[3] = isTargeted;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnTargetCardArrow"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnTargetCardArrow"])
					{
						Log.InfoFormat("Firing event OnTargetCardArrow -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, fromCard, toCard, isTargeted);
					}			}
		}

		public void OnMoveCard(Player player, Card card, Group fromGroup, Group toGroup, int oldIndex, int index, int oldX, int oldY, int x, int y, bool isScriptMove, string highlight, string markers)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnMoveCard");
			var args = new object[11];
			args[0] = player;
			args[1] = card;
			args[2] = fromGroup;
			args[3] = toGroup;
			args[4] = oldIndex;
			args[5] = index;
			args[6] = oldX;
			args[7] = oldY;
			args[8] = x;
			args[9] = y;
			args[10] = isScriptMove;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnMoveCard"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnMoveCard"])
					{
						Log.InfoFormat("Firing event OnMoveCard -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, card, fromGroup, toGroup, oldIndex, index, oldX, oldY, x, y, isScriptMove);
					}			}
		}

		public void OnPlayerGlobalVariableChanged(Player player, string name, string oldValue, string Value)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnPlayerGlobalVariableChanged");
			var args = new object[4];
			args[0] = player;
			args[1] = name;
			args[2] = oldValue;
			args[3] = Value;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnPlayerGlobalVariableChanged"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnPlayerGlobalVariableChanged"])
					{
						Log.InfoFormat("Firing event OnPlayerGlobalVariableChanged -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,player, name, oldValue, Value);
					}			}
		}

		public void OnGlobalVariableChanged(string name, string oldValue, string Value)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnGlobalVariableChanged");
			var args = new object[3];
			args[0] = name;
			args[1] = oldValue;
			args[2] = Value;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnGlobalVariableChanged"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnGlobalVariableChanged"])
					{
						Log.InfoFormat("Firing event OnGlobalVariableChanged -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,name, oldValue, Value);
					}			}
		}

		public void OnCardClick(Card card, int mouseButton, string[] keysDown)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnCardClick");
			var args = new object[3];
			args[0] = card;
			args[1] = mouseButton;
			args[2] = keysDown;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnCardClick"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnCardClick"])
					{
						Log.InfoFormat("Firing event OnCardClick -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,card, mouseButton, keysDown);
					}			}
		}

		public void OnCardDoubleClick(Card card, int mouseButton, string[] keysDown)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnCardDoubleClick");
			var args = new object[3];
			args[0] = card;
			args[1] = mouseButton;
			args[2] = keysDown;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnCardDoubleClick"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnCardDoubleClick"])
					{
						Log.InfoFormat("Firing event OnCardDoubleClick -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,card, mouseButton, keysDown);
					}			}
		}

		public void OnMarkerChanged(Card card, string markerName, int oldValue, int newValue, bool isScriptChange)
		{
			if(MuteEvents)return;
			Log.Info("Firing event OnMarkerChanged");
			var args = new object[5];
			args[0] = card;
			args[1] = markerName;
			args[2] = oldValue;
			args[3] = newValue;
			args[4] = isScriptChange;
	    
				if(Program.GameEngine.Definition.Events.ContainsKey("OnMarkerChanged"))
				{
					foreach(var e in Program.GameEngine.Definition.Events["OnMarkerChanged"])
					{
						Log.InfoFormat("Firing event OnMarkerChanged -> {0}",e.Name);
						engine.ExecuteFunction(e.PythonFunction,card, markerName, oldValue, newValue, isScriptChange);
					}			}
		}
	}
}
