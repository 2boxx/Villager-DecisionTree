using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : Node
{
    public delegate bool QuestionFunction();

    private QuestionFunction _question;
    private Node _positive;
    private Node _negative;

    public QuestionNode(QuestionFunction question, Node positive, Node negative)
    {
        _question = question;
        _positive = positive;
        _negative = negative;
    }

    public override void Action()
    {
        if (_question())
        {
            _positive.Action();
        }
        else
        {
            _negative.Action();
        }

    }
}