<p align="center">
  <a href="https://clodsire.nl" target="_blank">
    <picture>
      <source media="(prefers-color-scheme: dark)" srcset="https://clodsire.nl/img/brand/logo_white_text_dark.png">
      <img src="https://clodsire.nl/img/brand/logo_black_text_light.png" width="450px;">
    </picture>
  </a>
</p>

API for [Project Accessibility][1], which can be viewed at [https://api.clodsire.nl][2]. Below are the instructions on how to set up a test database for MySQL.

## Installation Steps for XAMPP (Can also be done with MySQL Workbench, but too lazy to add the steps)

1. **Clone the Repository:**
   Clone this repository to your local machine.

2. **Install XAMPP:**
   Download and install [XAMPP][3] on your system.

3. **Start XAMPP Services:**
   - Launch XAMPP.
   - Start both Apache and MySQL services from the XAMPP Control Panel.

4. **Access MySQL Admin Panel:**
   - Open a browser.
   - Go to `http://localhost/phpmyadmin/` or access the MySQL admin panel via XAMPP.

5. **Run the Program:**
   - Navigate to the project directory.
   - Run the application using either `dotnet run` or `dotnet watch`. Ensure you are in the `./API` directory before running.
   - Entity Framework Core will create the test database if it hasn't been created yet.

## Configuration (If Necessary)

- Navigate to the `appsettings.json` file in the project root.
- Update the `ConnectionStrings` section with your MySQL database details.

## Troubleshooting

- **Port Conflict for PHPMyAdmin:**
  - If you encounter port conflicts for PHPMyAdmin:
    - Open the XAMPP Control Panel.
    - Click on "Config" next to MySQL.
    - Change the "HTTP Port" to an available port (e.g., 3307).
    - Save and restart MySQL.

- If encountering migration issues, verify the correctness of the connection string.

[1]: https://clodsire.nl
[2]: https://api.clodsire.nl
[3]: https://www.apachefriends.org/download.html