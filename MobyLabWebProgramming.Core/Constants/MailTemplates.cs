namespace MobyLabWebProgramming.Core.Constants;

/// <summary>
/// Here we have a class that provides HTML template for mail bodies. You ami add or change the template if you like.
/// </summary>
public static class MailTemplates
{
    public static string UserAddTemplate(string name) => $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <title>Welcome Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 30px auto;
            background-color: #ffffff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #3c87be;
            color: white;
            padding: 20px;
            text-align: center;
        }}
        .content {{
            padding: 30px;
            color: #333;
        }}
        .content p {{
            margin-bottom: 15px;
            line-height: 1.6;
        }}
        .footer {{
            padding: 20px;
            text-align: center;
            font-size: 13px;
            color: #aaa;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Welcome to ReviewVerse!</h1>
        </div>
        <div class=""content"">
            <p><strong>Dear {name},</strong></p>
            <p>We're thrilled to welcome you to <strong>ReviewVerse</strong>, locul unde părerile tale contează cu adevărat!</p>
            <p>De acum poți lăsa recenzii, descoperi opiniile altora și contribui la o comunitate informată și sinceră.</p>
            <p>Dacă ai întrebări sau feedback, nu ezita să ne scrii. Suntem aici pentru tine!</p>
            <p>Toate cele bune,</p>
            <p><em>Razem Ioan<br/>Founder @ ReviewVerse</em></p>
        </div>
        <div class=""footer"">
            &copy; {DateTime.UtcNow.Year} ReviewVerse. All rights reserved.
        </div>
    </div>
</body>
</html>";
    
    
    public static string ReviewAddTemplate(string userName, string placeName, int rating, string content) => $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <title>Review Added</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 30px auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        h2 {{
            color: #3c87be;
        }}
        p {{
            color: #555;
            line-height: 1.6;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h2>Mulțumim pentru recenzie, {userName}!</h2>
        <p>Ai lăsat o recenzie pentru <strong>{placeName}</strong>:</p>
        <p><strong>Rating:</strong> {rating}/5</p>
        <p><strong>Conținut:</strong> {content}</p>
        <p>Recenzia ta contează și ajută alți utilizatori să ia decizii mai bune!</p>
        <p>Toate cele bune,<br/><em>Echipa ReviewVerse</em></p>
    </div>
</body>
</html>";

}
