using Unity.Netcode.Components;

public class ClientNetworkTransform : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative(){
        return false;
    }
}
