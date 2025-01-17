﻿using System;
using System.Collections.Generic;
using TQVaultAE.Domain.Contracts.Providers;
using TQVaultAE.Domain.Contracts.Services;
using TQVaultAE.Domain.Entities;
using TQVaultAE.Domain.Results;
using TQVaultAE.Logs;
using TQVaultAE.Presentation;

namespace TQVaultAE.Services
{
	public class StashService : IStashService
	{
		private readonly log4net.ILog Log = null;
		private readonly SessionContext userContext = null;
		private readonly IStashProvider StashProvider;
		private readonly IGamePathService GamePathResolver;

		public StashService(ILogger<StashService> log, SessionContext userContext, IStashProvider stashProvider, IGamePathService gamePathResolver)
		{
			this.Log = log.Logger;
			this.userContext = userContext;
			this.StashProvider = stashProvider;
			this.GamePathResolver = gamePathResolver;
		}



		/// <summary>
		/// Loads the transfer stash for immortal throne
		/// </summary>
		public LoadTransferStashResult LoadTransferStash()
		{
			var result = new LoadTransferStashResult();

			result.TransferStashFile = GamePathResolver.TransferStashFile;

			try
			{
				result.Stash = this.userContext.Stashes[result.TransferStashFile];
			}
			catch (KeyNotFoundException)
			{
				result.Stash = new Stash(Resources.GlobalTransferStash, result.TransferStashFile);
				result.Stash.IsImmortalThrone = true;

				try
				{
					result.StashPresent = StashProvider.LoadFile(result.Stash);
					if (result.StashPresent.Value)
						this.userContext.Stashes.Add(result.TransferStashFile, result.Stash);
				}
				catch (ArgumentException argumentException)
				{
					result.ArgumentException = argumentException;
				}
			}

			return result;
		}


		/// <summary>
		/// Loads the relic vault stash
		/// </summary>
		public LoadRelicVaultStashResult LoadRelicVaultStash()
		{
			var result = new LoadRelicVaultStashResult();

			result.RelicVaultStashFile = GamePathResolver.RelicVaultStashFile;

			// Get the relic vault stash
			try
			{
				result.Stash = this.userContext.Stashes[result.RelicVaultStashFile];
			}
			catch (KeyNotFoundException)
			{
				result.Stash = new Stash(Resources.GlobalRelicVaultStash, result.RelicVaultStashFile);
				result.Stash.CreateEmptySack();
				result.Stash.Sack.StashType = SackType.RelicVaultStash;

				try
				{
					result.StashPresent = StashProvider.LoadFile(result.Stash);
					if (result.StashPresent.Value)
					{
						result.Stash.Sack.StashType = SackType.RelicVaultStash;
						this.userContext.Stashes.Add(result.RelicVaultStashFile, result.Stash);
					}
				}
				catch (ArgumentException argumentException)
				{
					result.ArgumentException = argumentException;
				}
			}

			return result;
		}

		/// <summary>
		/// Attempts to save all modified stash files.
		/// </summary>
		/// <param name="stashOnError"></param>
		/// <exception cref="IOException">can happen during file save</exception>
		public void SaveAllModifiedStashes(ref Stash stashOnError)
		{
			// Save each stash as necessary
			foreach (KeyValuePair<string, Stash> kvp in this.userContext.Stashes)
			{
				string stashFile = kvp.Key;
				Stash stash = kvp.Value;

				if (stash == null) continue;

				if (stash.IsModified)
				{
					stashOnError = stash;
					GamePathResolver.BackupFile(stash.PlayerName, stashFile);
					StashProvider.Save(stash, stashFile);
				}
			}

		}

	}
}
