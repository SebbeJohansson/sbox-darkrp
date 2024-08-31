using GameSystems.Jobs;
using Sandbox.Events;

namespace Dxura.Darkrp;

public partial class PlayerState : Component
{
	/// <summary>
	/// Our local player on this client.
	/// </summary>
	public static PlayerState Local { get; private set; } = null!;

	/// <summary>
	/// Who owns this player state?
	/// </summary>
	[HostSync]
	[Property]
	public ulong SteamId { get; set; }

	/// <summary>
	/// The player's name, which might have to persist if they leave
	/// </summary>
	[HostSync]
	private string? SteamName { get; set; }

	/// <summary>
	/// The connection of this player
	/// </summary>
	public Connection Connection => Network.OwnerConnection;

	public bool IsConnected => Connection is not null && (Connection.IsActive || Connection.IsHost); //smh

	private string Name => SteamName ?? "";

	/// <summary>
	/// Name of this player
	/// </summary>
	public string DisplayName => $"{Name}{(!IsConnected ? " (Disconnected)" : "")}";

	/// <summary>
	/// What's our loadout?
	/// </summary>
	[RequireComponent]
	public PlayerLoadout Loadout { get; private set; } = null!;

	/// <summary>
	/// The job this player belongs to.
	/// </summary>
	[Property]
	[Group( "Setup" )]
	[HostSync]
	[Change( nameof(OnJobPropertyChanged) )]

	public JobResource Job { get; set; } = null!;

	/// <summary>
	/// Is this the local player for this client
	/// </summary>
	public bool IsLocalPlayer => !IsProxy && Connection == Connection.Local;

	/// <summary>
	/// Unique colour or team color of this player
	/// </summary>
	public Color PlayerColor => Job.Color;

	/// <summary>
	/// The main player
	/// </summary>
	[HostSync]
	[ValidOrNull]
	public Player? Player { get; set; }

	public void HostInit()
	{
		RespawnState = RespawnState.Immediate;

		SteamId = Connection.SteamId;
		SteamName = Connection.DisplayName;
	}

	[Authority]
	public void ClientInit()
	{
		Local = this;
	}

	public void Kick()
	{
		if ( Player.IsValid() )
		{
			Player.GameObject.Destroy();
		}

		GameObject.Destroy();
		// todo: actually kick em
	}

	public void AssignJob( JobResource team )
	{
		if ( !Networking.IsHost )
		{
			return;
		}

		Job = team;

		Scene.Dispatch( new JobAssignedEvent( this, team ) );
	}

	/// <summary>
	/// Called when <see cref="Job"/> changes across the network.
	/// </summary>
	private void OnJobPropertyChanged( JobResource before, JobResource after )
	{
		GameObject.Root.Dispatch( new JobChangedEvent( before, after ) );

		// Send this to the pawn too
		if ( Player.IsValid() )
		{
			Player.GameObject.Root.Dispatch( new JobChangedEvent( before, after ) );
		}
	}
}