using UnityEngine;

namespace Npc
{
    public class ShopNpc : NpcObj
    {
        public override void Interact()
        {
            PlayerInventory.Instance.ShowShop();
        }
        public override void Release()
        {
            base.Release();

            PlayerInventory.Instance.HideShop();
        }
    }
}

