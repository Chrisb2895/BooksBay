# DesiredConfiguration DSC - WebApp
# Configurazione per la verifica/installazione di una web app sulla macchina 'localhost'
# Versione 1.0.5
#
# La configurazione DSC WebAppInstallerDSC viene creata e utilizzata temporaneamente per creare la web app o il web service
# al termine delle operazioni viene dismessa
# Attenzione: non devono essere presenti configurazioni DSC già presenti e attive sugli stessi oggetti.
#
#  Riferimento: 
# DSC: https://docs.microsoft.com/it-it/powershell/scripting/dsc/overview/overview?view=powershell-7
# xWebAdministration: https://github.com/dsccommunity/xWebAdministration
# xPSDesiredStateConfiguration: https://github.com/dsccommunity/xPSDesiredStateConfiguration
#
# Sono presenti le seguenti variabili incluse tra # e {}
# WebAppName 			= nome tecnico dell'applicazione, senza spazi
# ExternalWebAppName 	= nome visibile da web 
# ServiceAccount 		= account con cui gira il servizio
# ServiceAccountPassw 	= password
# TargetFolder 			= cartella in cui si trovano i file

# Gestione credenziali, commentare se non necessario
# begin
#$password = '#{ServiceAccountPassw}#' | ConvertTo-SecureString -asPlainText -Force
#$username = '#{ServiceAccount}#'
#[PSCredential] $credential = New-Object System.Management.Automation.PSCredential($username,$password)
# end

#Configurazione DSC temporanea per installazione della webb app
Configuration WebAppInstallerDSC
{
    param
    (
        # Target nodes to apply the configuration
        [string[]]$NodeName = 'localhost'
    )
    
    Import-DscResource -ModuleName 'PSDesiredStateConfiguration'
    Import-DscResource -ModuleName 'xPSDesiredStateConfiguration'
	Import-DscResource -Module xWebAdministration
    
    Node $NodeName
    {
        #Esempio di cartella da creare / controllare che esista
        File Folder1
        {
             Type = 'Directory'
             DestinationPath = '#{TargetFolder}#'
             Ensure = "Present"
        }
	
		xWebAppPool AppPool
        {
            Ensure                  = 'Present'
            Name                    = '#{WebAppName}#AppPool'
            State                   = 'Started'
			# Credenziali
			#IdentityType 			= "SpecificUser"
			#Credential 				= $credential
            # Ad esempio Per eProcs
            # maxProcesses            = 5
            # Se gira con l'application pool identity serve questo
            # loadUserProfile         = $true
        }
		
		xWebApplication WebApp
        {
			DependsOn               = '[xWebAppPool]#{WebAppName}#AppPool'
            Ensure                  = 'Present'
            Name                    = '#{ExternalWebAppName}#'
            WebAppPool              = '#{WebAppName}#AppPool'
            Website                 = 'Default Web Site'
            PhysicalPath            = '#{TargetFolder}#'
			# Esempio gestione autenticazione 
            # AuthenticationInfo = MSFT_xWebApplicationAuthenticationInformation
            # {
			# 	Anonymous = $true
			# 	Basic = $false
			# 	Digest = $false
			# 	Windows = $false
            # }
			
        }
		
    }
}


$cd = @{
    AllNodes = @(
        @{
            NodeName = 'localhost'
			# Gestione credenziali, commentare se non necessario
            #PSDscAllowPlainTextPassword = $true
        }
    )
}

#Create the MOF
WebAppInstallerDSC -ConfigurationData $cd -NodeName 'localhost'

#Apply the configuration (stop on errors)
Start-DscConfiguration -Path .\WebAppInstallerDSC -Wait -Verbose -Force -ErrorAction 'Stop'

#Remove all (but does not roll back the changes)
Remove-DscConfigurationDocument -Stage Current, Pending, Previous -Verbose