namespace DragonFruit2.Generators;

public class A { }
public class A<T> : A { }
public class B : A<int> { }

internal static class Playspace
{
    extension(A)
    {
        public static string Hello()
        {
            return "Hello from A";
        }

    }
}


