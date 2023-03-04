pipeline {
  agent any

  stages {
    stage('Build') {
      steps {
        // Your build steps go here
      }
    }

    stage('Test') {
      steps {
        // Your test steps go here
      }
    }

    stage('Deploy') {
      steps {
        // Your deploy steps go here
      }
      
      post {
        success {
          script {
            def message = "Deployment successful!"
            def githubUrl = env.REPOSITORY_URL
            def commit = env.GIT_COMMIT
            def buildUrl = env.BUILD_URL
            githubNotify (
              status: 'SUCCESS',
              url: githubUrl,
              message: message,
              sha: commit,
              targetUrl: buildUrl
            )
          }
        }

        failure {
          script {
            def message = "Deployment failed!"
            def githubUrl = env.REPOSITORY_URL
            def commit = env.GIT_COMMIT
            def buildUrl = env.BUILD_URL
            githubNotify (
              status: 'FAILURE',
              url: githubUrl,
              message: message,
              sha: commit,
              targetUrl: buildUrl
            )
          }
        }
      }
    }
  }
}