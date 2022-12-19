namespace MyCarearApi.Services;

public static class CalculateSumma
{
 
 public static  string Calculate(decimal summa)
 {
     var inputSumma = summa.ToString();
     var array =  StringToMassiv(inputSumma);

     return Mapping(array);
 }
 

 public static string Mapping(int[] array)
 {

    string sozz = string.Empty;
    switch (array.Count())
   {
    case 1: sozz = BirLik(ref array);break;
    case 2: sozz = Onlik(ref array);break;
    case 3: sozz = Yuzlik(ref array);break;
    case 4: sozz = MingLik(ref array);break;
    case 5: sozz = OnMinglik(ref array);break;
    case 6: sozz = YuzMinglik(ref array);break;
    case 7: sozz = Millionlik(ref array);break;
    case 8: sozz = OnMillionlik(ref array);break;
    case 9: sozz = YuzMillionlik(ref array);break;
    case 10: sozz = Milliardlik(ref array);break;
    case 11: sozz = OnMilliardlik(ref array);break;
    case 12: sozz = YuzMilliardlik(ref array);break;
   }
    sozz = sozz.Replace("   "," ");
   System.Console.WriteLine(sozz.Replace("  "," "));

   return sozz;
 }

  public static int[] StringToMassiv(string son)
    {
        int [] son1 = new int[son.Length];

        for (int i = 0; i <son.Length; i++)
        {
            son1[i] = int.Parse(son.Substring(i,1));
        }

        return son1;
    }

   
  public static string BirLik(ref int[] array)
    {
      string Soz = string.Empty;
        if(array[0] != 0)
        {
            switch (array[0])
             {
             case 1: Soz+="bir"; break;
             case 2: Soz+="ikki"; break;
             case 3: Soz+="uch"; break;
             case 4: Soz+="to'rt"; break;
             case 5: Soz+="besh"; break;
             case 6: Soz+="olti"; break;
             case 7: Soz+="yetti"; break;
             case 8: Soz+="sakkiz"; break;
             case 9: Soz+="to'qqiz"; break; 
             
             }
            
       }
    return Soz;
    }

 public static string Onlik(ref int[] array)
    {
        if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return BirLik(ref yuzming);
    }
    else
    {
        string Sozz = string.Empty;
        if(array[1] == 0)
        {
            string Soz = string.Empty;
            switch (array[0])
             {
             case 1: Soz+="o'n"; break;
             case 2: Soz+="yigirma"; break;
             case 3: Soz+="o'ttiz"; break;
             case 4: Soz+="qirq"; break;
             case 5: Soz+="ellik"; break;
             case 6: Soz+="oltmish"; break;
             case 7: Soz+="yetmish"; break;
             case 8: Soz+="sakson"; break;
             case 9: Soz+="to'qson"; break;
             }
         Sozz = Soz;
        }
        if(array[1] != 0)
        {
            string Soz = string.Empty;
            switch (array[0])
             {
             case 1: Soz+="o'n"; break;
             case 2: Soz+="yigirma"; break;
             case 3: Soz+="o'ttiz"; break;
             case 4: Soz+="qirq"; break;
             case 5: Soz+="ellik"; break;
             case 6: Soz+="oltmish"; break;
             case 7: Soz+="yetmish"; break;
             case 8: Soz+="sakson"; break;
             case 9: Soz+="to'qson "; break;
             }
             int[] birlik = new int[1];
              birlik[0] =  array[1];
             Sozz = Soz+" "+BirLik(ref birlik);
        }
      return Sozz;
    }
    }

   public static string Yuzlik(ref int[] array)
   {
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return  Onlik(ref yuzming);
    }
    else
    {
    string Sozz = string.Empty;

    if(array[1] == 0 && array[2] == 0)
    {
        int[] birlik = new int[1];
        birlik[0] = array[0];
        Sozz =BirLik(ref birlik) + " yuz ";
    }
     if(array[1] == 0)
    {
        int[] birlik = new int[1];
        birlik[0] = array[0];
        Sozz =BirLik(ref birlik) + " yuz ";
        birlik[0] = array[2];
        Sozz = Sozz +" "+ BirLik(ref birlik);
    }
    if(array[1] != 0)
    {
        var birlik = new int[1];
        birlik[0] = array[0];
        Sozz =BirLik(ref birlik)+ " yuz ";
        var onlik = ToArray(1,array);
        Sozz =Sozz+" "+ Onlik(ref onlik);
    }
    return Sozz;
    }
   }

   public static string MingLik(ref int[] array)
   {
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return Yuzlik(ref yuzming);
    }
    else 
    {
    string Sozz = string.Empty;
     if(array[1] == 0)
     {
        var birlik = new int[1];
        birlik[0] = array[0];
        Sozz = BirLik(ref birlik)+" ming ";
        var onlik = ToArray(2,array);
        Sozz = Sozz +" "+ Onlik(ref onlik);
     }
     else
     {
        var birlik = new int[1];
        birlik[0] = array[0];
        Sozz = BirLik(ref birlik)+" ming ";
        var yuzlik = ToArray(1,array);
        Sozz = Sozz +" "+ Yuzlik(ref yuzlik);
     }
     return Sozz;
   }
   }

   public static string OnMinglik(ref int[] array)
   {
      if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return MingLik(ref yuzming);
    }
    else
    {
     string Sozz = string.Empty;
      var onlik = ToArrayFirst(1,array);
      Sozz =Onlik(ref onlik)+" ming ";
      var yuzlik = ToArray(2,array);
      Sozz =Sozz+" "+ Yuzlik(ref yuzlik);
     return Sozz;
    }
   }
   
   public static string YuzMinglik(ref int[] array)
   { 
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return OnMinglik(ref yuzming);
    }
    else
    {
    var yuzlik = ToArrayFirst(2,array);
    var yuzlik2 = ToArray(3,array);
    string Sozz = Yuzlik(ref yuzlik) + " ming " + Yuzlik(ref yuzlik2);
    return Sozz;
   }
   }

  public static string Millionlik(ref int[] array)
  {
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return YuzMinglik(ref yuzming);
    }
    else
    {
    var birlik = new int[1];
    birlik[0] = array[0];
    var yuzminglik = ToArray(1,array);
    string Sozz = BirLik(ref birlik) + " million " + YuzMinglik(ref yuzminglik);
    return Sozz;

    }
  }

  public static string OnMillionlik(ref int[] array )
  {
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return  Millionlik(ref yuzming);
    }
    else
    {
     string Sozz = string.Empty;
     var onlik = ToArrayFirst(1,array);
     var yuzminglik = ToArray(2,array);

     Sozz = Onlik(ref onlik)+" million "+YuzMinglik(ref yuzminglik);
    
    return Sozz;

    }
  }
  public static string YuzMillionlik(ref int[] array)
  {
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return OnMillionlik(ref yuzming);
    }
    else
    {
      var yuzlik = ToArrayFirst(2,array);
      var yuzminglik = ToArray(3,array);

      string Sozz = Yuzlik(ref yuzlik) + " million "+ YuzMinglik(ref yuzminglik);
     
     return Sozz;
    }
  }
  public static string Milliardlik(ref int[] array)
  {
    if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return  YuzMillionlik(ref yuzming);

    }
    else
    {
        var birlik = ToArrayFirst(0,array);
        var yuzmillionlik = ToArray(1,array);

        string Sozz = BirLik(ref birlik)+" milliard "+YuzMillionlik(ref yuzmillionlik);
        return Sozz;
    }
  }
  public static string OnMilliardlik(ref int[] array)
  {
     if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return  Milliardlik(ref yuzming);
    }
    else
    {
        var onlik = ToArrayFirst(1,array);
        var yuzmillionlik = ToArray(2,array);

        string Sozz = Onlik(ref onlik)+" milliard "+YuzMillionlik(ref yuzmillionlik);
        return Sozz;
    }
  }
  public static string YuzMilliardlik(ref int[] array)
  {
     if(array[0] == 0)
    {
        var yuzming = ToArray(1,array);

        return OnMilliardlik(ref yuzming);
    }
    else
    {
        var yuzlik = ToArrayFirst(2,array);
        var yuzmillionlik = ToArray(3,array);

        string Sozz = Yuzlik(ref yuzlik)+" milliard "+YuzMillionlik(ref yuzmillionlik);
        return Sozz;
    }
  }
   public static int[] ToArray(int boshIndex,int[] arry)
   {
     int[] array = new int[arry.Count()-boshIndex];
     for (int i = 0 ; i < arry.Count()-boshIndex; i++)
     {
       array[i] = arry[boshIndex+i];
     }
   return array;
   }
   public static int[] ToArrayFirst(int boshIndex,int[] arry)
   {
     int[] array = new int[arry.Count()-boshIndex];
     for (int i = 0 ; i<=boshIndex; i++)
     {
       array[i] = arry[i];
     }
   return array;
   }
}
