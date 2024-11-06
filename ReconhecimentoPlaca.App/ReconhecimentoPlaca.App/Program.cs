using Azure.AI.Vision.ImageAnalysis;
using Azure;

string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
string key = Environment.GetEnvironmentVariable("VISION_KEY");

ImageAnalysisClient client = new ImageAnalysisClient(
    new Uri(endpoint),
    new AzureKeyCredential(key));

string imagePath = "../../../images/carro_1.jfif"; 
byte[] imageBytes = File.ReadAllBytes(imagePath);
var binaryImage = new BinaryData(imageBytes);

ImageAnalysisResult result = client.Analyze(
    binaryImage,
    VisualFeatures.Caption | VisualFeatures.Read,
    new ImageAnalysisOptions { GenderNeutralCaption = true });

Console.WriteLine("Image analysis results:");
Console.WriteLine(" Caption:");
Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");

Console.WriteLine(" Read:");
foreach (DetectedTextBlock block in result.Read.Blocks)
    foreach (DetectedTextLine line in block.Lines)
    {
        Console.WriteLine($"   Line: '{line.Text}', Bounding Polygon: [{string.Join(" ", line.BoundingPolygon)}]");
        foreach (DetectedTextWord word in line.Words)
        {
            Console.WriteLine($"     Word: '{word.Text}', Confidence {word.Confidence.ToString("#.####")}, Bounding Polygon: [{string.Join(" ", word.BoundingPolygon)}]");
        }
    }