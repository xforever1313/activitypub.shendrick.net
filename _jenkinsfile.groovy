@Library( "X13JenkinsLib" )_

pipeline
{
    agent
    {
        label "master"
    }
    environment
    {
        DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
        DOTNET_NOLOGO = 'true'
    }
    options
    {
        skipDefaultCheckout( true );
    }
    stages
    {
        stage( 'clean' )
        {
            steps
            {
                cleanWs();
            }
        }
        stage( 'checkout' )
        {
            steps
            {
                checkout scm;
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: '*/main']],
                    extensions:[
                        [$class: 'SubmoduleOption', disableSubmodules: false, parentCredentials: false, recursiveSubmodules: true, reference: '', trackingSubmodules: false],
                        [$class: 'CleanCheckout'],
                        [$class: 'RelativeTargetDirectory', relativeTargetDir: 'keys']
                    ],
                    userRemoteConfigs: [
                        [credentialsId: 'shendrick.net', url: 'git@git.lan.thenaterhood.com:activitypub.shendrick.net/ClockBotKeys.git']
                    ]
                ]);
            }
        }

        stage( 'In Docker' )
        {
            agent
            {
                docker
                {
                    image 'mcr.microsoft.com/dotnet/sdk:6.0'
                    args "-e HOME='${env.WORKSPACE}' -e APP_BASE_KEY_DIRECTORY='${env.WORKSPACE}/keys'"
                    reuseNode true
                }
            }
            stages
            {
                stage( 'prepare' )
                {
                    steps
                    {
                        sh 'dotnet tool update Cake.Tool --tool-path ./Cake --version 3.0.0'
                        sh './Cake/dotnet-cake ./checkout/build.cake --showdescription'
                    }
                }
            
                stage( 'build' )
                {
                    steps
                    {
                        sh './Cake/dotnet-cake ./checkout/build.cake --target=build_pretzel'
                        sh './Cake/dotnet-cake ./checkout/build.cake --target=generate'
                    }
                }
            }
        }
    
        stage( 'deploy' )
        {
            steps
            {
                withCredentials(
                    [sshUserPrivateKey(
                        credentialsId: "shendrick.net",
                        usernameVariable: "SSHUSER",
                        keyFileVariable: "WEBSITE_KEY" // <- Note: WEBSITE_KEY must be in all quotes below, or rsync won't work if the path has whitespace.
                    )]
                )
                {
                    script
                    {
                        String verbose = "-v"; // Make "-v" for verbose mode.
                        String sshOptions = "-o BatchMode=yes -o StrictHostKeyChecking=accept-new -i \\\"${WEBSITE_KEY}\\\"";
                        sh "cd ./checkout/_site && rsync --rsh=\"ssh ${verbose} ${sshOptions}\" -az --delete --exclude \".well-known\" ./ ${ACTIVITYPUB_USER}@activitypub.shendrick.net:activitypub.shendrick.net";
                    }
                }
            }
        }
    }
    post
    {
        fixed
        {
            X13SendToTelegramWithCredentials(
                message: "${BUILD_TAG} has been fixed!  See: ${BUILD_URL}",
                botCredsId: "telegram_bot",
                chatCredsId: "telegram_chat_id"
            );
        }
        failure
        {
            X13SendToTelegramWithCredentials(
                message: "${BUILD_TAG} has failed.  See: ${BUILD_URL}",
                botCredsId: "telegram_bot",
                chatCredsId: "telegram_chat_id"
            );
        }
    }
}
