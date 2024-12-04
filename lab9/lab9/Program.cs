using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

public class Car 
{
    public string Model { get; set; }
    public Engine Motor { get; set; }
    public int Year { get; set; }

    public Car() { }
    public Car(string model, Engine motor, int year)
    {
        Model = model;
        Motor = motor;
        Year = year;
    }
}

public class Engine
{
    public double Displacement { get; set; }
    public int HorsePower { get; set; }
    public string Model { get; set; }

    public Engine() { }
    public Engine(double displacement, int horsePower, string model)
    {
        Displacement = displacement;
        HorsePower = horsePower;
        Model = model;
    }
}


class Program
{
    static void Main(string[] args)
    {
        List<Car> myCars = new List<Car>(){
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };

        LINQExp(myCars);

        serializeToXML(myCars, "CarsCollection.xml");
        List<Car> deserializedCars = deserializeFromXML("CarsCollection.xml");

        Console.WriteLine("\ndeserialized cars:");
        foreach (var car in deserializedCars)
        {
            Console.WriteLine($"Car(Model={car.Model}, Year={car.Year}, Engine(Displacement={car.Motor.Displacement}, " +
                $"HorsePower={car.Motor.HorsePower}, Model={car.Motor.Model}))");
        }


        XPathExp("CarsCollection.xml");
        createXmlFromLinq(myCars, "CarsFromLinq.xml");
        generateHTML(myCars, "template.html",  "carTable.html");
        ModifyXML("CarsCollection.xml", "modifiedCarsCollection.xml");
    }

    public static void LINQExp(List<Car> cars)
    {
        var query1 = from car in cars
                     where car.Model == "A6"
                     select new
                     {
                         engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol",
                         hppl = car.Motor.HorsePower / car.Motor.Displacement
                     };

        var query2 = from car in query1
                     group car by car.engineType into engineGroup
                     select new
                     {
                         EngineType = engineGroup.Key,
                         AvgHppl = engineGroup.Average(x => x.hppl)
                     };

        foreach (var group in query2)
        {
            Console.WriteLine($"{group.EngineType}: {group.AvgHppl}");
        }
    }
    private static void serializeToXML(List<Car> myCars, string filename)
    {
        XmlAttributeOverrides overrides = new XmlAttributeOverrides();
        XmlAttributes attributes = new XmlAttributes();
        attributes.XmlElements.Add(new XmlElementAttribute("engine"));
        overrides.Add(typeof(Car), "Motor", attributes);

        XmlAttributes carAttributes = new XmlAttributes();
        carAttributes.XmlType = new XmlTypeAttribute("car");
        overrides.Add(typeof(Car), carAttributes);

        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), overrides);

        using (StreamWriter writer = new StreamWriter(filename))
        {
            serializer.Serialize(writer, myCars);
        }
    }
    public static List<Car> deserializeFromXML(string filename)
    {
        XmlAttributeOverrides overrides = new XmlAttributeOverrides();
        XmlAttributes attributes = new XmlAttributes();
        attributes.XmlElements.Add(new XmlElementAttribute("engine"));
        overrides.Add(typeof(Car), "Motor", attributes);

        XmlAttributes carAttributes = new XmlAttributes();
        carAttributes.XmlType = new XmlTypeAttribute("car");
        overrides.Add(typeof(Car), carAttributes);

        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), overrides);
        using (StreamReader reader = new StreamReader(filename))
        {
            return (List<Car>)serializer.Deserialize(reader)!;
        }
    }
    public static void XPathExp(string filename)
    {
        XElement rootNode = XElement.Load(filename);

        double avgHP = (double)rootNode.XPathEvaluate("sum(//car[not(engine/Model = 'TDI')]" +
            "/engine/HorsePower) div count(//car[not(engine/Model = 'TDI')])");
        Console.WriteLine($"\nAverage power of cars with engines other than TDI: {avgHP}");
        
        IEnumerable<XElement> models = rootNode.XPathSelectElements("//car/Model[not(. = " +
            "preceding::car/Model)]");
        Console.WriteLine("\nCar models without repetitions:");
        foreach (var model in models)
        {
            Console.WriteLine(model.Value);
        }
    }
    private static void createXmlFromLinq(List<Car> myCars, string filename)
    {
        IEnumerable<XElement> nodes = myCars.Select(car =>
            new XElement("car",
                new XElement("Model", car.Model),
                new XElement("engine",
                new XElement("Displacement", car.Motor.Displacement),
                new XElement("HorsePower", car.Motor.HorsePower),
                new XElement("Model", car.Motor.Model)
                ),
                new XElement("Year", car.Year)
            )
        );

        XElement rootNode = new XElement("cars", nodes);
        rootNode.Save(filename);
    }

    private static void generateHTML(List<Car> myCars, string templateFilename, string filename)
    {
        XDocument template = XDocument.Load(templateFilename);
        XElement table = template.Descendants("table").First();
        table.Add(new XAttribute("border", "1"));

        XElement headerRow = new XElement("tr",
            new XElement("th", "Model"),
            new XElement("th", "Year"),
            new XElement("th", "Engine")
        );

        table.Add(headerRow);

        foreach (var car in myCars)
        {
            XElement carRow = new XElement("tr",
                new XElement("td", car.Model),
                new XElement("td", car.Year),
                new XElement("td",
                    new XElement("table",
                        new XElement("tr",
                            new XElement("td", "Displacement"),
                            new XElement("td", car.Motor.Displacement)),
                        new XElement("tr",
                            new XElement("td", "HorsePower"),
                            new XElement("td", car.Motor.HorsePower)),
                        new XElement("tr",
                            new XElement("td", "Model"),
                            new XElement("td", car.Motor.Model))
                    )
                )
            );

            table.Add(carRow);
        }

        template.Root.Element("head").Add(new XElement("style", @"
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th, td {
            border: 1px solid black;
            text-align: left;
            padding: 8px;
        }
        th {
            background-color: #ededeb;
        }
    "));

        template.Save(filename);
    }

    static void ModifyXML(string filename, string newFilename)
    {
        XDocument doc = XDocument.Load(filename);

        var cars = doc.Descendants("car");

        foreach (var car in cars)
        {
            var horsePowerElement = car.Element("engine")?.Element("HorsePower");
            if (horsePowerElement != null)
            {
                horsePowerElement.Name = "hp";
            }

            var yearElement = car.Element("Year");
            if (yearElement != null)
            {
                var modelElement = car.Element("Model");
                if (modelElement != null)
                {
                    modelElement.SetAttributeValue("year", yearElement.Value);
                    yearElement.Remove();
                }
            }
        }

        doc.Save(newFilename);
    }
}
