pipeline {
    agent any
    stages {
        stage("Verify tooling") {
            steps {
                sh '''
                    sudo docker version
                    sudo docker info
                    sudo docker-compose version
                '''
            }
        }
        stage('Start container') {
            steps {
                script {
                    try {
                        // Your build commands or steps that might fail
                        sh 'sudo docker stop workshop-management-system-jama'
                        sh 'sudo docker rm workshop-management-system-jama'
                    } catch (Exception e) {
                        echo "Cannot find container workshop-management-system-jama"
                    }
                }
                sh 'sudo docker-compose up --build --wait'
                sh 'sudo docker-compose ps'
            }
        }
    }
}
