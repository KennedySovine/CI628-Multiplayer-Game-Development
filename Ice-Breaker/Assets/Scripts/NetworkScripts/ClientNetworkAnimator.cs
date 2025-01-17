using Unity.Netcode.Components;

public class ClientNetworkAnimator : ClientNetworkAnimator{
    protected override bool OnIsServerAuthoritative(){
        return false;
    }
}