<p align="center">
  <a>
    <img src="https://github.com/CADindustries/container/blob/main/logos/package-custom.png" alt="Abdrakov.Solutions logo" width="256" height="256">
  </a>
</p>
<h1 align="center">Abdrakov.Container</h1>  

#### Prism Adapter:
[![Nuget](https://img.shields.io/nuget/v/Abdrakov.Container.PrismAdapter.svg)](http://nuget.org/packages/Abdrakov.Container.PrismAdapter)
[![Nuget](https://img.shields.io/nuget/dt/Abdrakov.Container.PrismAdapter.svg)](http://nuget.org/packages/Abdrakov.Container.PrismAdapter)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/CrackAndDie/Abdrakov.Container/blob/main/LICENSE)

#### Prism.Avalonia Adapter:
[![Nuget](https://img.shields.io/nuget/v/Abdrakov.Container.AvaloniaPrismAdapter.svg)](http://nuget.org/packages/Abdrakov.Container.AvaloniaPrismAdapter)
[![Nuget](https://img.shields.io/nuget/dt/Abdrakov.Container.AvaloniaPrismAdapter.svg)](http://nuget.org/packages/Abdrakov.Container.AvaloniaPrismAdapter)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/CrackAndDie/Abdrakov.Container/blob/main/LICENSE)

<h2>About:</h2>  

A package that provides registrations and resolves of services and other shite in the fast and lightweight container.

## Download for WPF with [Prism](https://github.com/PrismLibrary/Prism):  

<h4>.NET CLI:</h4>  

```dotnet add package Abdrakov.Container.PrismAdapter```

<h4>Package Reference:</h4>  

```<PackageReference Include="Abdrakov.Container.PrismAdapter" Version="*" />```   

## Download for Avalonia with [Prism.Avalonia](https://github.com/AvaloniaCommunity/Prism.Avalonia):   

<h4>.NET CLI:</h4>  

```dotnet add package Abdrakov.Container.AvaloniaPrismAdapter```

<h4>Package Reference:</h4>  

```<PackageReference Include="Abdrakov.Container.AvaloniaPrismAdapter" Version="*" />```   

<h2>Features:</h2>  

<h4>Attribute injections:</h4>  

All the registered shite could be resolved via *Injection* attribute (use the attribute only for properties and fields) like this:
```c#
private class NormalClass
{
    [Injection]
    InjectedClass TestClass { get; set; }
    [Injection]
    AnotherInjectedClass _anotherTestClass;
}
```

<h4>Constructor injections:</h4>  

Parametrised constructors could be used with *Abdrakov.Container*. For example after registering and resolving the class  
```c#
private class NormalClass
{
    private InjectedClass _testClass;
    private int _a;
    private string _b;

    public NormalClass(InjectedClass testClass, int a, string b = "awd")
    {
        _testClass = testClass;
        _a = a;
        _b = b;
    }
}
```
the *testClass* parameter would be resolved as usual (if it is not registered in the container then an instance of it would be created); the *a* parameter would have **default type** value (for Int32 is 0); the *b* parameter would have its **default parameter** value (in this case is "awd").  

<h4>Inheritance injections:</h4>  

The classed from which Your class is inherited would also be prepared for injections:  
```c#
private class InjectedClass
{
    internal int A { get; set; }
}

private class BaseClass
{
    [Injection]
    protected InjectedClass TestClass { get; set; }
}

private class NormalClass : BaseClass
{
}
```
So in this case after *NormalClass* registration and resolve, the *TestClass* property would also be injected.  

<h4>Recursive injections:</h4>  

There could be two classes that require injection of each other:
```c#
private class FirstClass
{
    [Injection]
    SecondClass InjectedClass { get; set; }
}

private class SecondClass
{
    [Injection]
    FirstClass InjectedClass { get; set; }
}
```
And this would work as expected!

<h2>Powered by:</h2>  

- *Abdrakov.Container*' logo - [Material Design Icons](https://materialdesignicons.com/)
- *Abdrakov.Container* is a rewritten part of [UnityContainer](https://github.com/unitycontainer/container)
