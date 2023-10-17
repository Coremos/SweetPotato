using UnityEngine;

public class Item : MonoBehaviour
{
    public int seq;

    // seq : 1 : ��Ʈ
    //     : 2 : �߰�����s
    public void GiveToMe()
    {
        Player player = GameManager.Instance.Player;
        if (player == null)
            return;

        switch (seq)
        {
            case 1:
                player.Heal(5);
                break;
            case 2:
                player.AddMaxSeed();
                break;
        }

        ObjectPoolManager.Instance.doDestroy(this.gameObject);
    }


}
