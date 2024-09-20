using SchoolSystem.Web.Helpers.Interfaces;

namespace SchoolSystem.Web.Helpers;

public class CreateMailHtmlHelper : ICreateMailHtmlHelper
{
    public string CreateMailBody(string name, string message)
    {
        var style = @"
<style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
            color: #333;
        }

        table {
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background-color: #fff;
            border-collapse: collapse;
        }

        td {
            padding: 20px;
            text-align: left;
        }

        /* Header Styles */
        .header {
            background-color: #4CAF50;
            color: white;
            text-align: center;
            padding: 10px 0;
        }

        .header h1 {
            margin: 0;
        }

        /* Main Content Styles */
        .content {
            padding: 20px;
        }

        .content h2 {
            color: #4CAF50;
        }

        .content p {
            line-height: 1.6;
        }

        /* Footer Styles */
        .footer {
            background-color: #333;
            color: white;
            text-align: center;
            padding: 10px 0;
            font-size: 12px;
        }

        .footer a {
            color: #4CAF50;
            text-decoration: none;
        }

        .footer a:hover {
            text-decoration: underline;
        }
    </style>
                    ";

        var body = $@"
                   <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>EduSmart</title>
    {style}
</head>
<body>
    <table>
        <!-- Header -->
        <tr>
            <td class=""header"">
                <h1>EduSmart</h1>
            </td>
        </tr>

        <!-- Main Content -->
        <tr>
            <td class=""content"">
              <p>Dear {name},</p>
              <!-- Message -->
                 {message}
                <p>If you have any questions, feel free to <a href=""mailto:costa0910supershop@gmail.com"">contact our support team</a>.</p>
                <p>Best Regards,<br>EduSmart Team</p>
            </td>
        </tr>

        <!-- Footer -->
        <tr>
            <td class=""footer"">
                <p>&copy; 2024 EduSmart. All rights reserved.</p>
                <p><a href=""http://localhost:5283/Home/Privacy"">Privacy Policy</a></p>
            </td>
        </tr>
    </table>
</body>
</html>
";

        return body;
    }
}
