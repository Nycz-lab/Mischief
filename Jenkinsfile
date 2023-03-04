@NonCPS
import groovy.json.JsonSlurper

// Parse request payload from Gtihub's webhook
def parseJSON(text) {
    return new groovy.json.JsonSlurperClassic().parseText(text)
}

// Parse request payload from webhook
def json = parseJSON(payload)

// Valudate pull_request
if (json.pull_request == null) {
    println("pull_request is null")
    return
}
// Dump pull_request
def url = json.pull_request.url
println("url: "+ url)

def json_number = json.number
println("json_number: "+ json_number)

def timeStamp=getLocalTime()
println("timeStamp: "+ timeStamp)

def json_branch = json.pull_request.head.ref
println("json_branch: "+ timeStamp)

def json_state = json.pull_request.state
println("json_state: "+ json_state)

def json_html = json.pull_request.html_url

def user_name=""
if (json.pull_request.user.login != null) {
  user_name = json.pull_request.user.login
}
println("user_name: "+ user_name)

// Validate PR's state must be open
if( "${json_state}" != "open" ) {
    echo "Pull request " + "${json_state}" + "."
    currentBuild.result = 'SUCCESS'
    return
}


// Pipeline name
def jobName="MischiefGame-pr"

// Machine's user Name
def machine_user_name="nickw"

// Your Github URL
def build_repo="git@github.com:Nycz-lab/Mischief.git"

// Your Unity Version
// Ex: "2020.1.15f2"
def unity_version="2021.3.19f1"


pipeline {
   // Master Jenkins
   agent any

   // Initialize environment params
   environment{
       UNITY_PATH="D:/programs/unityEditor/2021.3.19f1/Edito/Unity.exe"
       app_mode="mono"  // mono or IL2CPP
       repo="${build_repo}"
       branch="${json_branch}"
       pr_id="${json_number}"
       workingDir="/Users/${machine_user_name}/.jenkins/workspace/${json_branch}"
   }

    stages {
        stage('Clean up working space') {
            steps {
                echo  """del ${workingDir}\\${branch}
                """
            }
        }

        stage('Clone Branch') {
            steps {
              sh """git clone --branch ${branch} --depth 1 ${repo} ${workingDir}/${branch};\
                  cd ${workingDir}/${branch};\
                """
            }
        }

        stage('PlayMode Test') {
            steps {
              sh """cd ${workingDir}/${branch};\
                  ${UNITY_PATH} -batchmode -projectPath ${workingDir}/${branch} -runTests -testResults ${workingDir}/${branch}/CI/results.xml -testPlatform PlayMode -nographics -quit;\
                """
            }
        }

        stage('Build Android') {
            steps {
              script {
                   // BuildIL2CPPAndroid
                   if( "${app_mode}" != "mono" ) {
                     sh """cd ${workingDir}/${branch};\
                         mkdir builds
                         ${UNITY_PATH} -batchmode -projectPath ${workingDir}/${branch} -buildTarget Android -executeMethod BuilderUtility.BuildIL2CPPAndroid -nographics -quit;\
                         mv test-app.apk ./builds
                       """
                   } else {
                     // BuildMonoAndroid
                     sh """cd ${workingDir}/${branch};\
                         mkdir builds
                         ${UNITY_PATH} -batchmode -projectPath ${workingDir}/${branch} -buildTarget Android -executeMethod BuilderUtility.BuildMonoAndroid -nographics -quit;\
                         mv test-app.apk ./builds
                       """
                   }
               }
            }
        }

    }

}