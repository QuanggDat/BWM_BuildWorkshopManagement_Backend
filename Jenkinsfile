pipeline {
  agent any
  stages {
    stage("verify tooling") {
      steps {
        sh '''
          docker version
          docker info
          docker-compose version
        '''
      }
    }
    stage('Prune Docker data') {
      steps {
        sh 'docker system prune -a --volumes -f'
      }
    }
    stage('Start container') {
      steps {
	sh 'docker stop workshop-management-system'
        sh 'docker rm workshop-management-system'
        sh 'docker-compose up -d --no-color --wait'
        sh 'docker-compose ps'
      }
    }
    
  }
  
}
