pipeline {
    agent any

    stages {
        stage('Pull') {
            steps {
                git branch: 'main',
                    url: 'https://github.com/blyator/Mpesa-tracker-webpage.git'
            }
        }

        stage('Build Image') {
            steps {
                sh 'docker build -t mpesa-dashboard .'
            }
        }

        stage('Deploy') {
            steps {
                sh 'docker stop mpesa-dashboard || true'
                sh 'docker rm mpesa-dashboard || true'
                sh 'docker run -d --name mpesa-dashboard -p 4001:8080 --network n8n-stock-tracker_default --restart unless-stopped mpesa-dashboard'
            }
        }
    }

    post {
        success {
            echo 'Deployed successfully'
        }
        failure {
            echo 'Build failed'
        }
    }
}