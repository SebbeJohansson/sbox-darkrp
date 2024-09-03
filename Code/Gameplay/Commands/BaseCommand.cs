namespace Dxura.Darkrp.Commands;

public abstract class BaseCommand
{
	public virtual string Command { get; } = "";
	public virtual string Name { get; } = "";
	public virtual string Description { get; } = "";
	// TODO: Permissions
	// TODO: Arguments

	public virtual void Execute( string[] args )
	{
		Log.Info( $"Command {Name} executed with args: {string.Join( " ", args )}" );
	}
}
