using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animationPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject book1;
    public GameObject page1_plantTech;
    public GameObject Page2_tech;
    public GameObject cropbutton;
    public GameObject techbutton;
    public GameObject ShopCover;
    public GameObject BookCover;
    public animationPlay others;
    public Button cropNextPage1;

    public bool isOpenCrop = true;
    public bool isOpenTech = false;
    public bool isOpenBuild = false;

    public bool isProcessing = false;

    public bool isOpenBook = false;

    public void CropToTech ()
    {
        if(isOpenCrop && !isOpenTech && !isOpenBuild && !isProcessing)
        {
            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            book1.GetComponent<Animator>().Play("Flip");

            page1_plantTech.transform.SetSiblingIndex(9); // set lai ve 3
            Page2_tech.transform.SetSiblingIndex(8);// set lai ve 2
          
        }
        else if( !isOpenCrop && isOpenTech && !isOpenBuild && !isProcessing )
        {
            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            book1.GetComponent<Animator>().Play("FlipBackToCrop");
            page1_plantTech.transform.SetSiblingIndex(9); // set lai ve 3
            Page2_tech.transform.SetSiblingIndex(8);// set lai ve 2
        }
        else if( !isOpenCrop && !isOpenTech && isOpenBuild && !isProcessing)
        {
            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            book1.GetComponent<Animator>().Play("FlipTechAndCropToCrop");
            page1_plantTech.transform.SetSiblingIndex(9); // set lai ve 3
            Page2_tech.transform.SetSiblingIndex(8);// set lai ve 2
            Debug.Log("a");
        }
    }
    public void TechToBuildingAni()
    {
        if (isOpenTech && !isOpenCrop && !isOpenBuild && !isProcessing)
        {
            page1_plantTech.transform.SetSiblingIndex(8);
            Page2_tech.transform.SetSiblingIndex(9);

            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            book1.GetComponent<Animator>().Play("FlipTechToBuilding");
     

        }
        else if(!isOpenTech && !isOpenCrop && isOpenBuild && !isProcessing)
        {
           
            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            book1.GetComponent<Animator>().Play("FlipBackToTech");
            page1_plantTech.transform.SetSiblingIndex(8);
            Page2_tech.transform.SetSiblingIndex(9);

        }
        else if (!isOpenTech && isOpenCrop && !isOpenBuild && !isProcessing)
        {
           
            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime; //FlipStartToBuild dung hon
            book1.GetComponent<Animator>().Play("FlipStartToTech");
         
        }

    }

    public void OpenStore()
    {
        if(isOpenBook && !isProcessing)
        {
          

            BookCover.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime; //FlipStartToBuild dung hon
            BookCover.GetComponent<Animator>().Play("BookOpen");
            StartCoroutine(activeShopCover(2f));
            Debug.Log("a");
        }
        else if(!isOpenBook && !isProcessing)
        {
            page1_plantTech.transform.SetSiblingIndex(9); // set lai ve 3
            Page2_tech.transform.SetSiblingIndex(8);// set lai ve 2
            book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            book1.GetComponent<Animator>().Play("Start");

            StartCoroutine(deactiveShopCover(0.01f));

            isOpenBook = true;
          //  ShopCover.SetActive(false);
        
        }
    }


  
    public void cropRigtNextPage1()
    {
        book1.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        book1.GetComponent<Animator>().Play("rightCropPageNextPage1");
    }

    public void changeIndexPag1First()
    {
        page1_plantTech.transform.SetSiblingIndex(9);
        Page2_tech.transform.SetSiblingIndex(8);
    }
    public void changeIndex()
    {
        page1_plantTech.transform.SetSiblingIndex(8);
        Page2_tech.transform.SetSiblingIndex(9);
    }
    public void setIndexForThosePageA() 
    {
        page1_plantTech.transform.SetSiblingIndex(3); // set lai ve 3
        Page2_tech.transform.SetSiblingIndex(2);// set lai ve 2
        // Goi o giay thu 90 nhe dung quen toi oi
    }
    IEnumerator activeShopCover(float time)
    {
        yield return new WaitForSeconds(time);
   
        isOpenBook = false;
        ShopCover.SetActive(true);
       
        BookCover.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime; //FlipStartToBuild dung hon
        BookCover.GetComponent<Animator>().Play("New State");
        // Goi o giay thu 90 nhe dung quen toi oi
    }
    IEnumerator deactiveShopCover(float time)
    {
        yield return new WaitForSeconds(time);
     
        ShopCover.SetActive(false);

        // Goi o giay thu 90 nhe dung quen toi oi
    }
    IEnumerator  setIndexForThosePageB(float time)
    {
        yield return new WaitForSeconds(time);
        page1_plantTech.transform.SetSiblingIndex(2); // set lai ve 3
        Page2_tech.transform.SetSiblingIndex(3);// set lai ve 2
        // Goi o giay thu 90 nhe dung quen toi oi
    }
    public void setCropButtonLastIndex()
    {
       
        cropbutton.transform.SetAsLastSibling(); // cho xuong duoi tech
    
    }
    public void setCropButtonFirstIndex()
    {
        cropbutton.transform.SetAsFirstSibling(); // cho xuong duoi tech
    }
    public void setTechButtonLastIndex()
    {

        techbutton.transform.SetAsLastSibling(); // cho xuong duoi tech

    }
    public void setTechButtonFirstIndex()
    {
        techbutton.transform.SetAsFirstSibling(); // cho xuong duoi tech
    }

  
}
