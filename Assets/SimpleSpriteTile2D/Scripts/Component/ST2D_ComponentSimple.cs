using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moogle.SmartTile2D.TileManager;
namespace moogle.SmartTile2D
{
    public class ST2D_ComponentSimple : MonoBehaviour
    {

        public string tileTag = "None";
        public int zOrder = 0;
        public Material mat;
        public Sprite[] m_sprites;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void initSimpleTile()
        {
            if (mat == null)
            {
                mat = Resources.Load("Material/Default2D") as Material;
            }
            GameObject subObject = new GameObject();
            subObject.name = "CENTER";
            subObject.transform.SetParent(transform);
            SpriteRenderer render = subObject.AddComponent<SpriteRenderer>();
            render.sprite = m_sprites[0];
            render.sortingOrder = zOrder;
            render.transform.localScale = new Vector3(1f, 1f, 1f);
            render.transform.localPosition = new Vector3(0f, 0f, 0f);
            render.material = mat;
        }
        public void RandomReloadSprite()
        {
            var dst = transform.FindChild("CENTER").GetComponent<SpriteRenderer>();
            dst.sprite = m_sprites[Random.Range(m_sprites.GetLowerBound(0), m_sprites.GetUpperBound(0))];
        }

        public void ResetSortingOrder()
        {
            var renders = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renders)
            {
                r.sortingOrder = zOrder;
            }
        }

    }


}
