﻿#if(DEBUG)

using NUnit.Framework;

[TestFixture]
public class VerifyTest
{
    [Test]
    public void PeVerify1()
    {
        Verifier.Verify(AssemblyWeaver.BeforeAssemblyPath, AssemblyWeaver.AfterAssemblyPaths[0]);
    }

    [Test]
    public void PeVerify2()
    {
        Verifier.Verify(AssemblyWeaver.BeforeAssemblyPath, AssemblyWeaver.AfterAssemblyPaths[1]);
    }

    [Test]
    public void PeVerify3()
    {
        Verifier.Verify(AssemblyWeaver.MonoBeforeAssemblyPath, AssemblyWeaver.AfterAssemblyPaths[2]);
    }
}

#endif