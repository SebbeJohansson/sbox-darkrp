using Dxura.Darkrp.Commands;

namespace Dxura.Darkrp;

public class CommandsManager  : SingletonComponent<CommandsManager>
{
	[Sync]
	public List<BaseCommand> Commands { get; set; } = new();

	public CommandsManager()
	{
		// RegisterCommand( new BaseCommand() { Command ="", Name = "test", Description = "Test command" } );
		RegisterCommand( new LoremIpsum() );
		// UpdateCommandList();
	}

	public void RegisterCommand( BaseCommand command )
	{
		Commands.Add( command );
	}

	public List<BaseCommand> GetCommands()
	{
		// TODO: Add permission checks
		return Commands;
	}

	// [Broadcast ( NetPermission.HostOnly )]
	// public void UpdateCommandList() {
	// 	Chat.Instance.SetCommandList(GetCommands());
	// }
	
	// [Broadcast( NetPermission.HostOnly )]
	// protected void RpcRefresh()
	// {
	// 	Refresh();
	// }
	
	// 		using ( Rpc.FilterInclude( Connection.Host ) )
	// 		{
	// 			InflictKnifeDamage( tr.GameObject, tr.EndPosition, tr.Direction );
	// 		}
}
