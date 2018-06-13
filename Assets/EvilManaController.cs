﻿using UnityEngine;
using UnityEngine.UI;


public class EvilManaController : MonoBehaviour {
   
    #region Attributes
    [Header("Fragments UI info")]
    [SerializeField]
    private Image evilFragmentsFiller;
    [SerializeField]
    private Image evilFragmentsVoid;
    [SerializeField]
    private Text evilFragmentsNumber;

    [Header("Colors for the evil Circle")]
    [SerializeField]
    Color[] colors;

    private float maxPlayerEvil = 0;
    private float evilUnits = 0;
    #endregion

    #region MonoBehaviour Methods
    // Use this for initialization
    public void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(evilFragmentsNumber, "Error: No Text assigned to: " + gameObject.name);
        
        maxPlayerEvil = GameManager.instance.GetPlayer1().GetMaxEvilLevel();
        UnityEngine.Assertions.Assert.AreEqual(maxPlayerEvil / 10, colors.Length -1, "Error: Player's max evil level attribute and and numEvilColors must match (counting the +1 default evilColor" + gameObject.name);

        //Initialize evil UI resource
        evilUnits = NormalizeEvilNumber(maxPlayerEvil);
        evilFragmentsFiller.color = ColorCircleFiller((int) evilUnits);
        evilFragmentsVoid.color = ColorCircleFiller((int)evilUnits - 1);
        evilFragmentsNumber.text = NormalizeEvilNumber(maxPlayerEvil).ToString();

    }
    #endregion

    #region Public methods

    public void UpdateCurrentEvil(float currentEvil)
    {

        //Fill attribute has to be normalized to exist between 0 and 1
        evilFragmentsFiller.fillAmount = ((currentEvil % 10f) / 10) ;
        
        //Check if evil has increased or decreased to a new evil point
        if (int.Parse(evilFragmentsNumber.text) != NormalizeEvilNumber(currentEvil))
        {
            float normalizedEvil = NormalizeEvilNumber(currentEvil);
            evilFragmentsNumber.text = normalizedEvil.ToString();
            evilFragmentsFiller.color = ColorCircleFiller((int)normalizedEvil);
            evilFragmentsVoid.color = ColorCircleFiller((int)normalizedEvil - 1);
        }

    }
    #endregion

    #region Private methods
    private int NormalizeEvilNumber(float evil)
    {
        int normalizedEvilNumber = 0;
        //Divided / 10 because we are working two digits evil values, but the UI shows only one digit value. 
        normalizedEvilNumber = (int)(evil / 10);

        return normalizedEvilNumber;
    }

    private Color ColorCircleFiller(int currentEvil)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (i == currentEvil)
            {
                return colors[i];
            }
        }
        //representing empty color
        return Color.white; 
    }
    #endregion
}
