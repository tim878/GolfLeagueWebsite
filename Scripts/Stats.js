//var toggle = true;

function toggleVisibility(controlIDToChangeVisibility) 
{
    var element = document.getElementById(controlIDToChangeVisibility);
    if (isVisible(element)) 
    {
        element.style.display = 'none';
    }
    else
    {
        element.style.display = 'block';
    }
}

function toggleVisibility2(controlToChangeVisibility)
{
    var element = document.getElementById(controlIDToChangeVisibility.id);
    if (isVisible(element))
    {
        element.style.display = 'none';
    }
    else
    {
        element.style.display = 'block';
    }
}

function isVisible(elem)
{
    return elem.style.display == "block";
 //   return elem.offsetWidth > 0 || elem.offsetHeight > 0;
}

function ShowTable(control)
{
    HideAllControls();
    var element = document.getElementById(control.id);
    element.style.display = 'block';
}

function HideAllControls()
{
    document.getElementById('MainContent_Table_AverageScores').style.display = 'none';
    document.getElementById('MainContent_Table_LowestGrossSingleRoundScores').style.display = 'none';
    document.getElementById('MainContent_Table_LowestNetSingleRoundScores').style.display = 'none';
    document.getElementById('MainContent_Panel_AverageScoresByCourse').style.display = 'none';
    document.getElementById('MainContent_Table_Skins').style.display = 'none';
    document.getElementById('MainContent_Table_Birdies').style.display = 'none';
    document.getElementById('MainContent_Table_Pars').style.display = 'none';
    document.getElementById('MainContent_Table_Bogeys').style.display = 'none';
    document.getElementById('MainContent_Table_DoubleBogeys').style.display = 'none';
    document.getElementById('MainContent_Table_Eagles').style.display = 'none';
    document.getElementById('MainContent_Table_Par3Scoring').style.display = 'none';
    document.getElementById('MainContent_Table_Par4Scoring').style.display = 'none';
    document.getElementById('MainContent_Table_Par5Scoring').style.display = 'none';
    document.getElementById('MainContent_Panel_HoleScores').style.display = 'none';

    document.getElementById('MainContent_CourseStats').style.display = 'none';
    document.getElementById('MainContent_Graph').style.display = 'none';
    document.getElementById('MainContent_Panel_AllScores').style.display = 'none';
}