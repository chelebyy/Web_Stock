@echo off
echo Running SonarQube analysis...
dotnet-sonarscanner begin /k:"Stock" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="your-sonarqube-token" /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml" /d:sonar.coverage.exclusions="**/obj/**/*,**/bin/**/*,**/Migrations/**/*,**/tests/**/*"
dotnet build
dotnet-sonarscanner end /d:sonar.login="qp_03a71e6b4e112781457217c4a0e60579061b14e7"
echo SonarQube analysis completed.