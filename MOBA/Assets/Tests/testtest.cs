using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class testtest
{
    // A Test behaves as an ordinary method
    [Test]
    public void testtestSimplePasses()
    {
        // Use the Assert class to test conditions
        // Arrange
        test myMathSystem = new test();

        // Act
        int val = myMathSystem.add();

        // Assert
        Assert.That(val, Is.EqualTo(1));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator testtestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
