using Sandbox.Events;
namespace Dxura.Darkrp.Commands;

public class LoremIpsum : BaseCommand
{
	public override string Command { get; } = "loremipsum";
	public override string Name { get; } = "Lorem ipsum";
	public override string Description { get; } = "Sends text";
	
	public override void Execute( string[] args )
	{
		Log.Info( $"Command {Name} executed with args: {string.Join( " ", args )}" );
		RunCommandWithBroadcast();
		RunCommandOnlyOnHost();
		// TODO: Some how call this with a list of players that should have it
		RunCommandWithBroadcastToOnlySomePlayers( new List<Connection>() );
	}

	[Broadcast]
	public static void RunCommandWithBroadcast()
	{
		Log.Info( "Command ran with broadcast" );
	}

	[Broadcast( NetPermission.HostOnly )]
	public static void RunCommandOnlyOnHost()
	{
		Log.Info( "Command ran only on host" );
	}

	[Broadcast]
	public static void RunCommandWithBroadcastToOnlySomePlayers(List<Connection> connections)
	{
		using ( Rpc.FilterInclude( connections ) )
		{
			Log.Info( "Command ran only for players that are in the list." );
		}
	}

	// [Sync]
	// public static void RunCommandWithSync() {

	// }

	// public static void RunCommandWithBroadcastToOnlySomePlayers(List<Player> players)
	// {
	// 	RunCommandWithBroadcastToOnlySomePlayers( players.Select( x => x.Network.OwnerConnection ).ToList() );
	// }
}
