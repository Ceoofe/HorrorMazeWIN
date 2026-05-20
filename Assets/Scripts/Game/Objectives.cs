using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    public IEnumerator NewObjective(TMP_Text oldText, string newObj, int objNum) // Replace text
    {
        int index = oldText.text.LastIndexOf("•");

        if (objNum == 1)
        {
            string oldObj;
            string remain = oldText.text.Substring(index);
            if (index == 0)
            {
                oldObj = oldText.text.Substring(index);
                oldObj = "<s>" + oldObj + "</s>";
                oldText.text = oldObj;
                yield return new WaitForSeconds(2f);
                oldText.text = newObj;
                yield break;
            }
            else
            {
                oldObj = oldText.text.Substring(0, index);

                oldObj = "<s>" + oldObj + "</s>";
                oldText.text = oldObj + remain;
            }

            yield return new WaitForSeconds(2f);

            oldText.text = newObj + "\n" + remain;
        }
        else if (objNum == 2)
        {
            string remain;
            string oldObj;
            if (index == 0)
            {
                remain = oldText.text.Substring(index);
                oldText.text = remain + newObj;
            }
            else
            {
                remain = oldText.text.Substring(0, index);
                oldObj = oldText.text.Substring(index);
                oldObj = "<s>" + oldObj + "</s>";
                oldText.text = remain + oldObj;
            }

            yield return new WaitForSeconds(2f);

            oldText.text = remain + newObj;
        }
    }
}
