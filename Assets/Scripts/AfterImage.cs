using System;
using System.Linq;
using UnityEngine;

using UnityEngine.UI;

public class AfterImage : MonoBehaviour
{

   private bool isEnable = false;
   private bool isDone = false;
   [SerializeField] private Transform parent;
   private const int MaxShadow = 8;
   public  void Show()
   {
      isEnable = true;
      isDone = false;
      for (var i = 0; i < MaxShadow; i++)
      {
         children[i].GetComponent<Image>().color = new Color(1,1,1,0.15f*i/(1.0f * MaxShadow));
         children[i].position = transform.position;
         children[i].GetComponent<BoxCollider2D>().enabled = true;
      }
         
   }

   public void Hide()
   {
      isEnable = false;
      counter = 0;
   }

   private void Start()
   {
      CreateChildren();
   }

   private float timer = 0;
   private int counter = 0;
   private void Update()
   {
      timer += Time.deltaTime;
      if (isEnable)
      {
         if (timer >= 0.01f)
         {
            timer = 0;
               children[MaxShadow-1].position = transform.position;
               for (var i = 0; i < MaxShadow-1; i++)
                  children[i].position = children[i + 1].position;
         }
      }
      else
      {
         if (timer >= 0.001f)
         {
            timer = 0;
            if (counter >= MaxShadow) return;
            children[counter].GetComponent<Image>().color = Color.clear;
            children[counter].GetComponent<BoxCollider2D>().enabled = false;
            counter++;
         }
      }
      
   }

   Transform[] children = new Transform[MaxShadow];
   void CreateChildren()
   {
      while (parent.childCount>0)
         DestroyImmediate(parent.GetChild(0).gameObject);
      
      var sprite = GetComponent<Image>().sprite;
      for (var i = 0; i < MaxShadow; i++)
      {
         var child = new GameObject();
         child.transform.parent = parent;
         var img = child.AddComponent<Image>();
         img.sprite = sprite;
         img.color = new Color(1f, 1f, 1f, 0);
         child.SetActive(true);
         child.name = "Player Shadow " + i;
         child.transform.localScale = Vector3.one;
         child.tag = "Player";
         var rb = child.AddComponent<Rigidbody2D>();
         rb.bodyType = RigidbodyType2D.Dynamic;
         rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
         rb.gravityScale = 0;
         var bc = child.AddComponent<BoxCollider2D>();
         bc.offset = new Vector2(0, 0);
         bc.size = new Vector2(sprite.rect.width, sprite.rect.height);
         bc.enabled = false;
         children[i] = child.transform;
         children[i].position = transform.position;
         
      }
   }
}
