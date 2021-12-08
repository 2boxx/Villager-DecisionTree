using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleQuestionNode : Node
{
    public delegate EnviromentData.Enemies QuestionFunction();

    private QuestionFunction _question;
    private Node _resultA;
    private Node _resultB;
    private Node _resultC;

    public TripleQuestionNode(QuestionFunction question, Node resultA, Node resultB, Node resultC)
    {
        _question = question;
        _resultA = resultA;
        _resultB = resultB;
        _resultC = resultC;
    }

    public override void Action()
    {
        var currentType = _question();
        switch (currentType)
        {
            case EnviromentData.Enemies.Archers: //Si hay arqueros hacer establos
                _resultA.Action();
                break;
            case EnviromentData.Enemies.Infantry: //Si hay infanteria hacer arqueros
                _resultB.Action();
                break;
            case EnviromentData.Enemies.Cavalry: //Si hay caballeria, hacer infanteria
                _resultC.Action();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

